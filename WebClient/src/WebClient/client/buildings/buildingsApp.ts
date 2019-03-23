///<amd-module name="buildings/buildingsApp"/>

import { Router, IRouterOptions, RouterMode } from "utils/router";
import { Create } from "buildings/create/create";
import { BuildingsList } from "buildings/list/list";
import { Building } from "buildings/building/building";
import { SubContractor } from "buildings/building/companies/subContractor/subContractor";

import * as ko from "knockout";

export class BuildingsApp {
    public router: Router;
    public create: ko.Observable<Create>;
    public buildingsList: ko.Observable<BuildingsList>;
    public building: ko.Observable<Building>;
    public subContractor: ko.Observable<SubContractor>;

    constructor() {
        this.create = ko.observable(null);
        this.buildingsList = ko.observable(null);
        this.building = ko.observable(null);
        this.subContractor = ko.observable(null);
        this.router = new Router(this.createRouterOptions());
        this.router.add("buildings/:buildingId/create", buildingId => this.showCreate(buildingId));
        this.router.add("buildings/:buildingId/subContractor", buildingId => this.newSubContractor(buildingId));
        this.router.add("buildings/:buildingId", buildingId => this.showBuilding(buildingId));
        this.router.add("buildings", () => this.showList());
        this.router.start();
    }

    private showList(): void {
        this.hideAll();
        this.buildingsList(new BuildingsList());
    }

    private showCreate(buildingId: number) {
        this.hideAll();
        this.create(new Create(buildingId));
    }

    private showBuilding(buildingId: number) {
        this.hideAll();
        this.building(new Building(buildingId));
    }

    private newSubContractor(buildingId: number) {
        this.hideAll();
        this.subContractor(new SubContractor(buildingId));
    }

    private createRouterOptions(): IRouterOptions {
        return {
            mode: RouterMode.History,
            root: window.location.host
        }
    }

    public  hideAll(): void {
        if (this.create() != null)
            this.create().dispose();
        this.create(null);

        if (this.building() != null)
            this.building().dispose();
        this.building(null);

        if (this.buildingsList() != null)
            this.buildingsList().dispose();
        this.buildingsList(null);

        if (this.subContractor() != null)
            this.subContractor().dispose();
        this.subContractor(null);
    }

    public dispose(): void {
    }
}