///<amd-module name="home/home"/>

import { Buildings } from "home/buildings/buildings";
import { Notifications } from "home/notifications/notifications";

export class Home {
    public buildings: Buildings;
    public notifications: Notifications;

    constructor() {
        this.buildings = new Buildings();
        this.notifications = new Notifications();
    }

    public dispose() {
        this.buildings.dispose();
        this.notifications.dispose();
    }
}