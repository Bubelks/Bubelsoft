///<amd-module name="company/companyApp"/>

import { Router, IRouterOptions, RouterMode } from "utils/router";
import { Company } from "company/company";

import * as ko from "knockout";

export class CompanyApp {
    private company: ko.Observable<Company>;
    public router: Router;

    constructor() {
        this.company = ko.observable(null);
        this.router = new Router(this.createRouterOptions());
        this.router.add("company/:companyId", companyId => this.showCompany(companyId));
        this.router.add("company", () => this.showCompany(null));
        this.router.start();
    }

    private showCompany(companyId: number): void {
        this.company(new Company(companyId));
    }

    private createRouterOptions(): IRouterOptions {
        return {
            mode: RouterMode.History,
            root: window.location.host
        }
    }
}