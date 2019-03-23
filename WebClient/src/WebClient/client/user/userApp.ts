///<amd-module name="user/userApp"/>

import { Router, IRouterOptions, RouterMode } from "utils/router";
import { RegisterUser } from "user/register/register";
import { LogIn } from "user/logIn/logIn";

import * as ko from "knockout";
import * as navigator from "utils/navigator";

export class UserApp {
    private registerUser: ko.Observable<RegisterUser>;
    public logIn: ko.Observable<LogIn>;
    public router: Router;

    constructor() {
        this.logIn = ko.observable(null);
        this.registerUser = ko.observable(null);
        this.router = new Router(this.createRouterOptions());
        this.router.add("user/register/:userId", userId => this.showRegister(userId));
        this.router.add("user/logIn", () => this.showLogIn());
        this.router.start();
    }


    private showLogIn(): void {
        this.hideAll();
        this.logIn(new LogIn(() => this.authorize()));
    }

    private showRegister(userId: number): void {
        this.hideAll();
        this.registerUser(new RegisterUser(userId));
    }

    public authorize(): void {
        navigator.navigate("home");
    }

    private hideAll(): void {
        if (this.logIn() !== null) {
            this.logIn().dispose();
            this.logIn(null);
        }
        if (this.registerUser() !== null) {
            this.registerUser().dispose();
            this.registerUser(null);
        }
    }
    private createRouterOptions(): IRouterOptions {
        return {
            mode: RouterMode.History,
            root: window.location.host
        }
    }
}