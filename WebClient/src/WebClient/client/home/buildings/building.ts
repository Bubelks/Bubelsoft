﻿///<amd-module name="home/buildings/building"/>

import * as navigator from "utils/navigator";

export class Building {
    public name: string;
    public ownedByMy: boolean;
    public company: ICompany;
    public showNotifications: (buildingName: string) => void;

    constructor(base: IBuilding, showNotifications: (buildingName: string) => void) {
        this.name = base.name;
        this.ownedByMy = base.ownedByMy;
        this.company = base.company;
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
        navigator.navigate(`company/${this.company.id}`);
    }
}

export interface IBuilding {
    name: string;
    ownedByMy: boolean;
    company: ICompany;
}

interface ICompany {
    name: string;
    id: number;
}