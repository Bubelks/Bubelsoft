///<amd-module name="app"/>

import { Start } from "start/start";
import { Home } from "home/home";
import { CompanyApp } from "company/companyApp";
import { BuildingsApp } from "buildings/buildingsApp";
import { UserApp } from "user/userApp";
import { Router, IRouterOptions, RouterMode } from "utils/router";

import * as ko from "knockout";
import * as navigator from "utils/navigator";

export class App
{
    public static apiUrl = "http://localhost:13567/api";

    public start: ko.Observable<Start>;

    public home: ko.Observable<Home>;
    public companyApp: CompanyApp;
    public buildingsApp: BuildingsApp;
    public userApp: UserApp;
    public router: Router;

    public useBuildingsApp: ko.Observable<boolean>;
    public useCompanyApp: ko.Observable<boolean>;
    public useUserApp: ko.Observable<boolean>;
    public showNavBar: ko.Observable<boolean>;

    constructor() {
        this.start = ko.observable(new Start());
        this.home = ko.observable(null);
        this.useBuildingsApp = ko.observable(false);
        this.useCompanyApp = ko.observable(false);
        this.useUserApp = ko.observable(false);
        this.showNavBar = ko.observable(true);

        this.buildingsApp = new BuildingsApp();
        this.companyApp = new CompanyApp();
        this.userApp = new UserApp();
        this.router = new Router(this.createRouterOptions());
        this.router.add("home", () => this.showHome());
        this.router.add("company", () => this.showCompany());
        this.router.add("buildings", () => this.showBuildings());
        this.router.add("user", () => this.showUser());
        this.router.start();
    }

    public unauthorize(): void {
        navigator.navigate("user/logIn");
    }

    public openUserMenu(): void {
        $("nav .dropdown .user-avatar").toggleClass("show");
        $("nav .dropdown .dropdown-menu").toggleClass("show");
    }

    public logOut(): void {
        document.cookie = "bubelsoftToken=; expires=Thu, 01 Jan 1970 00:00:01 GMT;";
        navigator.navigate("logIn");
    }

    public goTo = (url: string): void => {
        navigator.navigate(url);
    }

    private showCompany(): void {
        this.hideAll();
        this.useCompanyApp(true);
    }

    private showBuildings(): void {
        this.hideAll();
        this.useBuildingsApp(true);
    }

    private showUser(): void {
        this.hideAll();
        this.showNavBar(false);
        this.useUserApp(true);
    }

    private showHome(): void {
        this.hideAll();
        this.home(new Home());
    }

    private hideAll(): void {
        this.showNavBar(true);
        if (this.home() !== null) {
            this.home().dispose();
            this.home(null);
        }
        this.start(null);
        this.useCompanyApp(false);
        this.useBuildingsApp(false);
        this.useUserApp(false);
    }

    private createRouterOptions(): IRouterOptions {
        return {
            mode: RouterMode.Hash,
            root: window.location.host
        }
    }
}

export class AppSingleton {
    private static instance: App;

    public static setInstance(app: App): void {
        this.instance = app;
    }

    public static getInstance(): App {
        return this.instance;
    }
}