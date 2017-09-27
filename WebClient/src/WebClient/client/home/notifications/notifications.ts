///<amd-module name="home/notifications/notifications"/>

import * as ko from "knockout";

export class Notifications {
    public newNotifications: ko.ObservableArray<string>;

    constructor() {
        this.newNotifications = ko.observableArray(["Notification1", "Notification2"]);
    }

    public dispose(): void {
        
    }
}