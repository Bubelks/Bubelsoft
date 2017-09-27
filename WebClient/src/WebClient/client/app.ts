///<amd-module name="app"/>

import { Buildings } from "home/buildings/buildings";
import { Notifications } from "home/notifications/notifications";
import { LogIn } from "logIn/logIn";

import * as ko from "knockout";
export class App
{
    public static apiUrl = "http://localhost:5000/api";

    public readonly buildings: Buildings;
    public readonly notifications: Notifications;
    public readonly logIn: LogIn;
    public readonly authorized: ko.Observable<boolean>;

    constructor() {
        this.buildings = new Buildings();
        this.notifications = new Notifications();
        this.logIn = new LogIn(() => this.authorize());
        this.authorized = ko.observable(true);
    }

    public unauthorize(): void {
        this.authorized(false);
    }

    public authorize(): void {
        this.authorized(true);
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