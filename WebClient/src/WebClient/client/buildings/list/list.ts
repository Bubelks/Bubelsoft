///<amd-module name="buildings/list/list "/>

import { Building, IBuilding } from "home/buildings/building";

import * as rest from "utils/communication/rest";
import * as ko from "knockout";
import * as navigator from "utils/navigator";

export class BuildingsList {
    public mainContractBuildings: ko.ObservableArray<Building>;
    public subContractBuildings: ko.ObservableArray<Building>;

    constructor() {
        this.mainContractBuildings = ko.observableArray([]);
        this.subContractBuildings = ko.observableArray([]);

        this.getBuildings().done((data: IBuilding[]) => {
            this.mainContractBuildings(data.filter(d => d.ownedByMy).map(d => new Building(d, () => {})));
            this.subContractBuildings(data.filter(d => !d.ownedByMy).map(d => new Building(d, () => {})));
        });
    }

    public goToBuilding(building: Building): void {
        navigator.navigate(`buildings/${building.id}`);
    }

    private getBuildings(): JQueryPromise<IBuilding[]> {
        return rest.get("buildings", "");
    }

    public dispose(): void {
        
    }
}