///<amd-module name="app"/>

import { Buildings } from "home/buildings/buildings";
import { Notifications } from "home/notifications/notifications";

export class App
{
    public static apiUrl = "http://localhost:13567/api";

    public readonly buildings: Buildings;
    public readonly notifications: Notifications;

    constructor() {
        this.buildings = new Buildings();
        this.notifications = new Notifications();
    }
}