///<amd-module name="home/buildings/buildings"/>

import * as ko from "knockout";
import * as rest from "utils/communication/rest";

export class Buildings {
    public buildings: ko.ObservableArray<IBuilding>;

    constructor() {
        this.buildings = ko.observableArray([]);
        rest.get("buildings", "").done(
            data => {
                this.buildings(data);
            }
        );
    }
}

interface IBuilding {
    name: string;
    ownedByMy: boolean;
}