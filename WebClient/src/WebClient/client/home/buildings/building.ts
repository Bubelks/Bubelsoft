///<amd-module name="home/buildings/building"/>

import { Report } from "buildings/building/report/report";

import * as navigator from "utils/navigator";
import * as rest from "utils/communication/rest";
import * as ko from "knockout";

export class Building {
    public name: string;
    public ownedByMy: boolean;
    public companyName: string;
    public showNotifications: (buildingName: string) => void;
    public rolesString: string;
    public id: number;
    public canReport: boolean;

    public newReport: ko.Observable<Report>;

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
        this.canReport = base.userBuildingRoles.filter(r => r === UserBuildingRole.Reporter).length > 0;
        this.rolesString = this.getRolesString(base.userBuildingRoles);

        this.newReport = ko.observable(null);
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

    public goToBuilding(_, event): void {
        if ($(event.target).hasClass("company-name")) return;
        if ($(event.target).hasClass("grid-button")) return;
        if ($(event.target).hasClass("fa")) return;

        navigator.navigate(`buildings/${this.id}`);
    }

    public openAddReport(): void {
        this.newReport(new Report(this.id));
    }

    public addReport(): void {
        const dto = this.newReport().getDto();
        rest.put("building", `${this.id}/report`, dto);
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

interface ICompany {
    name: string;
    id: number;
}