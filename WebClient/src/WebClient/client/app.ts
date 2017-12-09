///<amd-module name="app"/>

import { Home } from "home/home";
import { LogIn } from "logIn/logIn";
import { Company } from "company/company";
import { Router, IRouterOptions, RouterMode } from "utils/router";

import * as ko from "knockout";

export class App
{
    public static apiUrl = "http://localhost:5000/api";

    public home: ko.Observable<Home>;
    public logIn: ko.Observable<LogIn>;
    public company: ko.Observable<Company>;
    public buildings: ko.Observable<Home>;
    public router: Router;

    constructor() {
        this.home = ko.observable(null);
        this.logIn = ko.observable(null);
        this.company = ko.observable(null);
        this.buildings = ko.observable(null);
        this.router = new Router(this.createRouterOptions());
        this.router.add("home", () => this.showHome());
        this.router.add("logIn", () => this.showLogIn());
        this.router.add("company", () => this.showCompany());
        this.router.add("buildings", () => this.showBuildings());
        this.router.start();
        this.goTo("home");
    }

    public unauthorize(): void {
        this.router.navigate("logIn");
    }

    public authorize(): void {
        this.router.navigate("home");
    }

    public openUserMenu(): void {
        $("nav .dropdown .user-avatar").toggleClass("show");
        $("nav .dropdown .dropdown-menu").toggleClass("show");
    }

    public logOut(): void {
        document.cookie = "bubelsoftToken=; expires=Thu, 01 Jan 1970 00:00:01 GMT;";
        this.router.navigate("logIn");
    }

    public goTo = (url: string): void => {
        this.router.navigate(url);
    }

    private showCompany(): void {
        this.hideAll();
        this.company(new Company());
    }

    private showBuildings(): void {
        this.hideAll();
        this.buildings(new Home());
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
        if (this.company() !== null) {
            this.company().dispose();
            this.company(null);
        }
        if (this.buildings() !== null) {
            this.buildings().dispose();
            this.buildings(null);
        }
    }

    private createRouterOptions(): IRouterOptions {
        return {
            mode: RouterMode.History,
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