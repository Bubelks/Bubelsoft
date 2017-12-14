///<amd-module name="buildings/create/steps/building"/>

import { IStep } from "buildings/create/create";
import * as ko from "knockout";

export class BuildingStep implements IStep {
    private nextStep: IStep;
    private backStep: IStep;

    public readonly active: ko.Observable<boolean>;

    constructor() {
        this.active = ko.observable(false);
    }

    public valid(): boolean { throw new Error("Not implemented"); }

    public next(): IStep {
        return this.nextStep;
    }

    public back(): IStep {
        return this.backStep;
    }

    public activate(active: boolean): void {
        this.active(active);
    }

    public header(): string { return "Building" }

    public setNext(next: IStep) {
        this.nextStep = next;
    }

    public setBack(back: IStep) {
        this.backStep = back;
    }
}