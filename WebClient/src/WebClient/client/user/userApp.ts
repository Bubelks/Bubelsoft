///<amd-module name="user/userApp"/>

import { Router, IRouterOptions, RouterMode } from "utils/router";
import { RegisterUser } from "user/register/register";

import * as ko from "knockout";

export class UserApp {
    private registerUser: ko.Observable<RegisterUser>;
    public router: Router;

    constructor() {
        this.registerUser = ko.observable(null);
        this.router = new Router(this.createRouterOptions());
        this.router.add("user/register/:userId", userId => this.showRegister(userId));
        this.router.start();
    }

    private showRegister(userId: number): void {
        this.registerUser(new RegisterUser(userId));
    }

    private createRouterOptions(): IRouterOptions {
        return {
            mode: RouterMode.History,
            root: window.location.host
        }
    }
}