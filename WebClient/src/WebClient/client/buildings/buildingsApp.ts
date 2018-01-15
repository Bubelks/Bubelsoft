///<amd-module name="buildings/building"/>

import { Router, IRouterOptions, RouterMode } from "utils/router";
import { Create } from "buildings/create/create";
import { BuildingsList } from "buildings/list/list";

import * as ko from "knockout";

export class BuildingsApp {
    public router: Router;
    public create: ko.Observable<Create>;
    public buildingsList: ko.Observable<BuildingsList>;

    constructor() {
        this.create = ko.observable(null);
        this.buildingsList = ko.observable(null);
        this.router = new Router(this.createRouterOptions());
        this.router.add("buildings/:buildingId/create", buildingId => this.showCreate(buildingId));
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

        if (this.buildingsList() != null)
            this.buildingsList().dispose();
        this.buildingsList(null);
    }

    public dispose(): void {
    }
}