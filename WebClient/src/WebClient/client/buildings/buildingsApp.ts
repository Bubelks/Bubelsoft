///<amd-module name="buildings/buildingsApp"/>

import { Router, IRouterOptions, RouterMode } from "utils/router";
import { Create } from "buildings/create/create";

import * as ko from "knockout";

export class BuildingsApp {
    public router: Router;
    public create: ko.Observable<Create>;
    constructor() {
        this.create = ko.observable(null);
        this.router = new Router(this.createRouterOptions());
        this.router.add("buildings/:buildingId/create", buildingId => this.showCreate(buildingId));
        this.router.start();
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
    }

    public dispose(): void {
    }
}