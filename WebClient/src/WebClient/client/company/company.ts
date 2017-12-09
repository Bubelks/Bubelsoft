///<amd-module name="company/company"/>

import * as ko from "knockout";

export class Company {
    public name: string;
    public visibleWorkers: ko.ObservableArray<IUser>;

    constructor() {
        this.name = "Company";
        this.visibleWorkers = ko.observableArray([
            {
                name: "Name1",
                firstName: "FirstName1",
                lastName: "LastName1",
                email: "mail1@mail.com",
                tel: "500500500",
                role: "admin"
            },
            {
                name: "Name1",
                firstName: "FirstName1",
                lastName: "LastName1",
                email: "mail1@mail.com",
                tel: "500500500",
                role: "admin"
            },
            {
                name: "Name1",
                firstName: "FirstName1",
                lastName: "LastName1",
                email: "mail1@mail.com",
                tel: "500500500",
                role: "admin"
            },
            {
                name: "Name1",
                firstName: "FirstName1",
                lastName: "LastName1",
                email: "mail1@mail.com",
                tel: "500500500",
                role: "admin"
            },
            {
                name: "Name1",
                firstName: "FirstName1",
                lastName: "LastName1",
                email: "mail1@mail.com",
                tel: "500500500",
                role: "admin"
            }]);
    }

    public dispose(): void {
        
    }
}

interface IUser {
    name: string;
    firstName: string;
    lastName: string;
    email: string;
    tel: string;
    role: string;
}