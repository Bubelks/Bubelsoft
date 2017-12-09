///<amd-module name="home/buildings/buildings"/>

import { Building, IBuilding } from "home/buildings/building";

import * as ko from "knockout";

export class Buildings {
    public visibleBuilding: ko.Computed<IBuilding[]>;
    public contractTypes: ko.ObservableArray<IContractType>;
    public selectedType: ko.Observable<IContractType>;

    private buildings: Building[];

    constructor(buildings: IBuilding[], showNotifications: (buildingName: string) => void) {
        this.buildings = buildings.map(b => new Building(b, showNotifications));

        this.contractTypes = ko.observableArray([
            { type: ContractType.All, displayName: "All" },
            { type: ContractType.SubContract, displayName: "Sub-Contract" },
            { type: ContractType.MainContract, displayName: "Main-contract" }
        ]);
        this.selectedType = ko.observable<IContractType>(this.contractTypes()[0]);

        this.visibleBuilding = ko.computed<IBuilding[]>(() => {
            switch (this.selectedType().type)
            {
                case ContractType.SubContract:
                    return this.buildings.filter(b => !b.ownedByMy);
                case ContractType.MainContract:
                    return this.buildings.filter(b => b.ownedByMy);
                default:
                    return this.buildings;
            }
        });
    }

    public dispose(): void {
        
    }
}

interface IContractType {
    type: ContractType;
    displayName: string;
}

enum ContractType {
    All,
    SubContract,
    MainContract
}