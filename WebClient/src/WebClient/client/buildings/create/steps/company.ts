///<amd-module name="buildings/create/steps/company"/>

import { IStep } from "buildings/create/create";
import * as ko from "knockout";

export class CompanyStep implements IStep {
    private readonly nextStep: IStep;

    public readonly active: ko.Observable<boolean>;

    constructor(nextStep: IStep) {
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
}