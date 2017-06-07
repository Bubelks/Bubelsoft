///<amd-module name="app"/>

import * as ko from "knockout";

export class App
{
    constructor() {
        
        this.notifications = ["Notification1", "Notification2"];
        this.isNotification = ko.observable(this.notifications.length > 0);

    }
    public notifications: string[];
    public isNotification: ko.Observable<boolean>;
    
}