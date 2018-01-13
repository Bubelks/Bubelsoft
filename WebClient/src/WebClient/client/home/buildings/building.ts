///<amd-module name="home/buildings/building"/>

import * as navigator from "utils/navigator";

export class Building {
    public name: string;
    public ownedByMy: boolean;
    public companyName: string;
    public showNotifications: (buildingName: string) => void;

    private companyId: number;

    constructor(base: IBuilding, showNotifications: (buildingName: string) => void) {
        this.name = base.name;
        this.ownedByMy = base.ownedByMy;
        this.companyName = base.companyName;
        this.companyId = base.companyId;
        this.showNotifications = showNotifications;
    }

    public icon(): string {
        if (this.ownedByMy)
            return "fa-circle";
        return "fa-circle-o";
    }

    public contractType(): string {
        if (this.ownedByMy)
            return "Main-Contract";
        return "Sub-Contract";
    }

    public goToCompany(): void {
        navigator.navigate(`company/${this.companyId}`);
    }
}

export interface IBuilding {
    name: string;
    ownedByMy: boolean;
    companyName: string;
    companyId: number;
}

interface ICompany {
    name: string;
    id: number;
}