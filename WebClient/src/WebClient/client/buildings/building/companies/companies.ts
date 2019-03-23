///<amd-module name="buildings/building/companies/companies"/>

import * as navigator from "utils/navigator";
import * as ko from "knockout";
import * as rest from "utils/communication/rest";

export class Companies {
    private readonly buildingId: number;

    public readonly subContractors: ko.ObservableArray<BuildingCompany>;
    public readonly mainContractor: ko.Observable<BuildingCompany>;

    public readonly avaiableCompanies: ko.ObservableArray<ISelectValue>;
    public readonly newCompany: ko.Observable<ISelectValue>;
    public readonly newCompanyEmail: ko.Observable<string>;

    constructor(companies: IBuildingCompany[], buildingId: number) {
        this.buildingId = buildingId;
        this.avaiableCompanies = ko.observableArray([]);
        this.newCompany = ko.observable(null);
        this.newCompanyEmail = ko.observable(null);
        this.mainContractor = ko.observable(new BuildingCompany(companies.filter(d => d.mainContract)[0], this.buildingId, this.closeCompanies));
        this.subContractors = ko.observableArray(companies.filter(d => !d.mainContract).map(d => new BuildingCompany(d, this.buildingId, this.closeCompanies)));
    }

    public addSubcontractor(): void {
        rest.get("company", "")
            .done(data => this.avaiableCompanies(data.filter(d => d.value !== this.mainContractor().id &&
                this.subContractors().filter(sc => sc.id === d.value).length === 0)));
    }

    public confirmAdding(): void {
        if (this.newCompanyEmail() !== null)
            rest.put("building", `${this.buildingId}/companies/add`, this.newCompanyEmail());
        else
            rest.post("building", `${this.buildingId}/companies/add`, this.newCompany().value);
    }

    private closeCompanies = () => {
        this.mainContractor().isOpen(false);
        this.subContractors().forEach(c => c.isOpen(false));
    }

    public dispose(): void {

    }
}

class BuildingCompany {
    isOpen: ko.Observable<boolean>;
    id: number;
    name: string;
    workers: IBuildingwWorker[];
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
    private closeCompaies: () => void;
    private buildingId: number;

    public avaiableWorkers: ko.ObservableArray<ISelectValue>;
    public avaiableRoles: ko.ObservableArray<ISelectValue>;
    public newWorker: ko.Observable<ISelectValue>;
    public addingRole: ko.Observable<ISelectValue>;
    public roles: ko.Observable<string>;
    public canAddWorker: boolean;
    private roleIds: Array<number>;

    constructor(base: IBuildingCompany, buildingId: number, closeCompanies: () => void) {
        this.buildingId = buildingId;
        this.closeCompaies = closeCompanies;
        this.isOpen = ko.observable(false);
        this.id = base.id;
        this.name = base.name;
        this.canAddWorker = base.canAddWorker;
        this.workers = base.workers.map(w => {
            return {
                userId: w.userId,
                displayName: w.displayName,
                userBuildingRoles: this.getRolesString(w.userBuildingRoles as UserBuildingRole[])
            }
        });

        this.avaiableWorkers = ko.observableArray([]);
        this.newWorker = ko.observable(null);
        this.avaiableRoles = ko.observableArray(this.buildingRolesMap.map(rm => ({
            value: rm.key,
            displayValue: rm.value
        })));
        this.newWorker = ko.observable(this.avaiableRoles[0]);
        this.roles = ko.observable("");
        this.roleIds = new Array();
        this.addingRole = ko.observable(null);
    }

    public addWorker(): void {
        rest.get("company", `${this.id}/workers`)
            .done(data => this.avaiableWorkers(data.filter(d => this.workers.filter(sc => sc.userId === d.value).length === 0)));
    }

    public addRole(): void {
        if (this.roles() !== "")
            this.roles(this.roles() + ", ");

        this.roles(this.roles() + this.addingRole().displayValue);
        this.roleIds.push(this.addingRole().value);
        this.avaiableRoles.remove(this.addingRole());
        this.addingRole(null);
    }

    public confirmAddWorker(): void {
        rest.post("building",
            `${this.buildingId}/worker/add`,
            {
                userId: this.newWorker().value,
                userBuildingRoles: this.roleIds
            }).
            done(() => {
                this.avaiableWorkers([]);
                this.newWorker(null);
                this.avaiableRoles(this.buildingRolesMap.map(rm => ({
                    value: rm.key,
                    displayValue: rm.value
                })));
                this.newWorker(this.avaiableRoles[0]);
                this.roles("");
                this.roleIds = new Array();
                this.addingRole(null);
            });
    }

    public toggleWorkers = (company: BuildingCompany) => {
        if (!company.isOpen()) {
            this.closeCompaies();
        }

        company.isOpen(!company.isOpen());
    }

    public goToCompany(company: BuildingCompany): void {
        navigator.navigate(`company/${company.id}`);
    }

    private getRolesString(roles: UserBuildingRole[]): string {
        let result = this.buildingRolesMap.filter(r => r.key === roles[0])[0].value;
        for (var i = 1; i < roles.length; i++)
            result += `, ${this.buildingRolesMap.filter(r => r.key === roles[i])[0].value}`;
        return result;
    }
}

export interface IBuildingCompany {
    id: number;
    name: string;
    mainContract: boolean;
    canAddWorker: boolean;
    workers: IBuildingwWorker[];
}

interface IBuildingwWorker {
    userId: number;
    displayName: string;
    userBuildingRoles: UserBuildingRole[] | string;
}