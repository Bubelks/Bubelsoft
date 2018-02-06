///<amd-module name="buildings/create/steps/company"/>

import { IStep, ICompany } from "buildings/create/create";
import * as ko from "knockout";

export class CompanyStep implements IStep {
    private id: number;
    private name: ko.Observable<string>;
    private nip: ko.Observable<string>;
    private phoneNumber: ko.Observable<string>;
    private email: ko.Observable<string>;
    private city: ko.Observable<string>;
    private postCode: ko.Observable<string>;
    private street: ko.Observable<string>;
    private placeNumber: ko.Observable<string>;

    private readonly nextStep: IStep;

    public readonly active: ko.Observable<boolean>;

    constructor(nextStep: IStep = null, company: ICompany= null) {
        if (company != null) {
            this.id = company.id;
            this.name = ko.observable(company.name);
            this.nip = ko.observable(company.nip);
            this.phoneNumber = ko.observable(company.phoneNumber);
            this.email = ko.observable(company.email);
            this.city = ko.observable(company.city);
            this.postCode = ko.observable(company.postCode);
            this.street = ko.observable(company.street);
            this.placeNumber = ko.observable(company.placeNumber);

            this.nextStep = nextStep;
            this.active = ko.observable(false);
        } else {
            this.id = 0;
            this.name = ko.observable(null);
            this.nip = ko.observable(null);
            this.phoneNumber = ko.observable(null);
            this.email = ko.observable(null);
            this.city = ko.observable(null);
            this.postCode = ko.observable(null);
            this.street = ko.observable(null);
            this.placeNumber = ko.observable(null);

            this.nextStep = null;
            this.active = ko.observable(false);
        }
    }

    public valid(): boolean { throw new Error("Not implemented"); }

    public next(): IStep {
        return this.nextStep;
    }
    public back(): IStep {
        return null;
    }

    public activate(active: boolean): void {
        this.active(active);
    }
    public header(): string { return "Company" }

    public getDto(): ICompany {
        return {
            id: this.id,
            name: this.name(),
            nip: this.nip(),
            phoneNumber: this.phoneNumber(),
            email: this.email(),
            city: this.city(),
            postCode: this.postCode(),
            street: this.street(),
            placeNumber: this.placeNumber()
    }
    }
}