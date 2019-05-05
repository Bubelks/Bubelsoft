///<amd-module name="home/home"/>

import { Buildings } from "home/buildings/buildings";
import { IBuilding } from "home/buildings/building";
import { Notifications } from "home/notifications/notifications";

import * as rest from "utils/communication/rest";
import * as ko from "knockout";

export class Home {
    public buildings: ko.Observable<Buildings>;
    public notifications: ko.Observable<Notifications>;


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