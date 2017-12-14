///<amd-module name="buildings/create/steps/estimate"/>

import { IStep } from "buildings/create/create";

import * as ko from "knockout";

export class EstimateStep implements IStep {
    private readonly backStep: IStep;

    public readonly active: ko.Observable<boolean>;

    constructor(backStep: IStep) {
        this.backStep = backStep;
        this.active = ko.observable(false);
    }

    public valid(): boolean { throw new Error("Not implemented"); }

    public next(): IStep {
        return null;
    }

    public back(): IStep {
        return this.backStep;
    }

    public activate(active: boolean): void {
        this.active(active);
    }

    public header(): string { return "Estimate" }
}