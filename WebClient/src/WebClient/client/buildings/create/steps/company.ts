///<amd-module name="buildings/create/steps/company"/>

import { IStep, ICompany } from "buildings/create/create";
import * as ko from "knockout";

export class CompanyStep implements IStep {
    private id: number;
    private name: ko.Observable<string>;
    private nip: ko.Observable<string>;
    private phoneNumber: ko.Observable<string>;
    private eMail: ko.Observable<string>;
    private city: ko.Observable<string>;
    private postCode: ko.Observable<string>;
    private street: ko.Observable<string>;
    private placeNumber: ko.Observable<string>;

    private readonly nextStep: IStep;

    public readonly active: ko.Observable<boolean>;

    constructor(nextStep: IStep, company: ICompany) {
        this.id = company.id;
        this.name = ko.observable(company.name);
        this.nip = ko.observable(company.nip);
        this.phoneNumber = ko.observable(company.phoneNumber);
        this.eMail = ko.observable(company.eMail);
        this.city = ko.observable(company.city);
        this.postCode = ko.observable(company.postCode);
        this.street = ko.observable(company.street);
        this.placeNumber = ko.observable(company.placeNumber);

        this.nextStep = nextStep;
        this.active = ko.observable(false);
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
            eMail: this.eMail(),
            city: this.city(),
            postCode: this.postCode(),
            street: this.street(),
            placeNumber: this.placeNumber()
    }
    }
}