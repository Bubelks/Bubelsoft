///<amd-module name="home/notifications/notifications"/>

import * as ko from "knockout";

export class Notifications {
    private allNotifcations: ko.ObservableArray<INotification>;

    public visibleNotifications: ko.Computed<INotification[]>;
    public newNotificationsCount: ko.Observable<number>;

    public notificationTypes: ko.ObservableArray<INotificationType>;
    public selectedType: ko.Observable<INotificationType>;

    public buildingNames: ko.ObservableArray<string>;
    public selectedBuildingName: ko.Observable<string>;
    public buildingsNamesVisible: ko.Computed<boolean>;
    public specifyBuilding: ko.Observable<boolean>;

    constructor(buildingNames: Array<string>) {
        this.allNotifcations = ko.observableArray([]);

        this.notificationTypes = ko.observableArray([
            { type: NotificationType.All, displayName: "All" },
            { type: NotificationType.Building, displayName: "Building" },
            { type: NotificationType.Company, displayName: "Company" }
        ]);
        this.selectedType = ko.observable<INotificationType>(this.notificationTypes()[0]);

        this.buildingNames = ko.observableArray(buildingNames);
        this.selectedBuildingName = ko.observable("");
        this.buildingsNamesVisible = ko.computed<boolean>(() => this.selectedType().type === NotificationType.Building);
        this.specifyBuilding = ko.observable(false);

        this.newNotificationsCount = ko.observable(this.allNotifcations().filter(n => n.isNew).length);

        this.visibleNotifications = ko.computed<INotification[]>(() => {
            switch (this.selectedType().type) {
                case NotificationType.Building:
                    return this.allNotifcations()
                        .filter(n =>
                            n.type === NotificationType.Building &&
                            (!this.specifyBuilding() || this.selectedBuildingName() === n.source));
                case NotificationType.Company:
                    return this.allNotifcations().filter(n => n.type === NotificationType.Company);
                default:
                    return this.allNotifcations();
            }

        });
    }

    public toggleSpecifyBuilding(): void {
        this.specifyBuilding(!this.specifyBuilding());
    }

    public showFor = (buildingName: string): void => {
        this.selectedBuildingName(buildingName);
        this.selectedType(this.notificationTypes().filter(n => n.type === NotificationType.Building)[0]);
        this.specifyBuilding(true);
    }

    public dispose(): void {
        this.visibleNotifications.dispose();
        this.buildingsNamesVisible.dispose();
    }
}

interface INotification {
    name: string;
    source: string;
    content: string;
    isNew: boolean;
    type: NotificationType;
}

interface INotificationType {
    type: NotificationType;
    displayName: string;
}
enum NotificationType {
    All,
    Building,
    Company
}