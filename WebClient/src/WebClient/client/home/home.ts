///<amd-module name="home/home"/>

import { Buildings } from "home/buildings/buildings";
import { IBuilding } from "home/buildings/building";
import { Notifications } from "home/notifications/notifications";

import * as rest from "utils/communication/rest";
import * as ko from "knockout";

export class Home {
    public buildings: ko.Observable<Buildings>;
    public notifications: ko.Observable<Notifications>;

    constructor() {
        this.buildings = ko.observable(new Buildings([], null));
        this.notifications = ko.observable(new Notifications([]));

        this.getBuildings()
            .then((buildings) => {
                this.notifications(new Notifications(buildings.map(b => b.name)));
                this.buildings(new Buildings(buildings, this.notifications().showFor));
            });
    }

    private getBuildings(): JQueryPromise<IBuilding[]> {
        return rest.get("buildings", "");
    }

    public dispose() {
        if(this.buildings())
            this.buildings().dispose();
        if (this.notifications())
            this.notifications().dispose();
    }
}