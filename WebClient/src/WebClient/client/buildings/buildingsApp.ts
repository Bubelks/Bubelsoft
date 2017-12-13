///<amd-module name="buildings/building"/>

import { Router, IRouterOptions, RouterMode } from "utils/router";
import { Create } from "buildings/create/create";

import * as ko from "knockout";

export class BuildingsApp {
    public router: Router;
    public create: ko.Observable<Create>;

    constructor() {
        this.create = ko.observable(null)
        this.router = new Router(this.createRouterOptions());
        this.router.add("buildings/:buildingId/create", buildingId => this.showCreate(buildingId));
        this.router.start();
    }

    public goToCreate(): void {
        this.router.navigate("#buildings/1/create");
    }

    private showCreate(buildingId: number) {
        console.log(buildingId);
        this.create(new Create());
    }

    private createRouterOptions(): IRouterOptions {
        return {
            mode: RouterMode.History,
            root: window.location.host
        }
    }
}