///<amd-module name="app"/>

import { Home } from "home/home";
import { LogIn } from "logIn/logIn";
import { Router, IRouterOptions, RouterMode } from "utils/router";

import * as ko from "knockout";

export class App
{
    public static apiUrl = "http://localhost:5000/api";

    public home: ko.Observable<Home>;
    public logIn: ko.Observable<LogIn>;
    public router: Router;

    constructor() {
        this.home = ko.observable(null);
        this.logIn = ko.observable(null);
        this.router = new Router(this.createRouterOptions());
        this.router.add("home", () => this.goToHome());
        this.router.add("logIn", () => this.goToLogIn());
        this.router.start();
        this.router.navigate("home");
    }

    public unauthorize(): void {
        this.router.navigate("logIn");
    }

    public authorize(): void {
        this.router.navigate("home");
    }

    public openUserMenu(): void {
        $("nav .dropdown .dropdown-menu").toggleClass("show");
    }

    public logOut(): void {
        document.cookie = "bubelsoftToken=; expires=Thu, 01 Jan 1970 00:00:01 GMT;";
        this.router.navigate("logIn");
    }

    private goToHome(): void {
        this.home(new Home());

        if (this.logIn() !== null) {
            this.logIn().dispose();
            this.logIn(null);
        }
    }

    private goToLogIn(): void {
        this.logIn(new LogIn(() => this.authorize()));

        if (this.home() !== null) {
            this.home().dispose();
            this.home(null);
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