///<amd-module name="home/buildings/building"/>

import * as navigator from "utils/navigator";

export class Building {
    public name: string;
    public ownedByMy: boolean;
    public companyName: string;
    public showNotifications: (buildingName: string) => void;
    public rolesString: string;
    public id: number;

    private companyId: number;
    private buildingRolesMap: IMap[] = [
        {
            key: UserBuildingRole.Admin,
            value: "Administrator"
        },
        {
            key: UserBuildingRole.Reporter,
            value: "Reporter"
        }
    ];

    constructor(base: IBuilding, showNotifications: (buildingName: string) => void) {
        this.id = base.id;
        this.name = base.name;
        this.ownedByMy = base.ownedByMy;
        this.companyName = base.companyName;
        this.companyId = base.companyId;
        this.showNotifications = showNotifications;
        this.rolesString = this.getRolesString(base.userBuildingRoles);
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

    private getRolesString(roles: UserBuildingRole[]): string {
        let result = this.buildingRolesMap.filter(r => r.key === roles[0])[0].value;
        for (var i = 1; i < roles.length; i++)
            result += `, ${this.buildingRolesMap.filter(r => r.key === roles[i])[0].value}`;
        return result;
    }
}

export interface IBuilding {
    id: number;
    name: string;
    ownedByMy: boolean;
    companyName: string;
    companyId: number;
    userBuildingRoles: UserBuildingRole[];
}

export enum UserBuildingRole {
    Admin,
    Reporter
}

interface ICompany {
    name: string;
    id: number;
}

interface IMap {
    key: UserBuildingRole;
    value: string;
}