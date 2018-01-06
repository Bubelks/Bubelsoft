///<amd-module name="app"/>

import { Home } from "home/home";
import { LogIn } from "logIn/logIn";
import { CompanyApp } from "company/companyApp";
import { BuildingsApp } from "buildings/buildingsApp";
import { Router, IRouterOptions, RouterMode } from "utils/router";

import * as ko from "knockout";
import * as navigator from "utils/navigator";

export class App
{
    public static apiUrl = "http://localhost:5000/api";

    public home: ko.Observable<Home>;
    public logIn: ko.Observable<LogIn>;
    public companyApp: CompanyApp;
    public buildingsApp: BuildingsApp;
    public router: Router;

    public useBuildingsApp: ko.Observable<boolean>;
    public useCompanyApp: ko.Observable<boolean>;

    constructor() {
        this.home = ko.observable(null);
        this.logIn = ko.observable(null);
        this.useBuildingsApp = ko.observable(false);
        this.useCompanyApp = ko.observable(false);

        this.buildingsApp = new BuildingsApp();
        this.companyApp = new CompanyApp();
        this.router = new Router(this.createRouterOptions());
        this.router.add("home", () => this.showHome());
        this.router.add("logIn", () => this.showLogIn());
        this.router.add("company", () => this.showCompany());
        this.router.add("buildings", () => this.showBuildings());
        this.router.start();
    }

    public unauthorize(): void {
        navigator.navigate("logIn");
    }

    public authorize(): void {
        navigator.navigate("home");
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

    private showHome(): void {
        this.hideAll();
        this.home(new Home());
    }

    private showLogIn(): void {
        this.hideAll();
        this.logIn(new LogIn(() => this.authorize()));
    }

    private hideAll(): void {
        if (this.logIn() !== null) {
            this.logIn().dispose();
            this.logIn(null);
        }
        if (this.home() !== null) {
            this.home().dispose();
            this.home(null);
        }
        this.useCompanyApp(false);
        this.useBuildingsApp(false);
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