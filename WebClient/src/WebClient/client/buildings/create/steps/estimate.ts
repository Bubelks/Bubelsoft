///<amd-module name="buildings/create/steps/estimate"/>

import { IStep } from "buildings/create/create";

import * as ko from "knockout";

export class EstimateStep implements IStep {
    private readonly backStep: IStep;
    private file: any;
    public readonly active: ko.Observable<boolean>;
    public readonly badFile: ko.Observable<boolean>;

    constructor(backStep: IStep) {
        this.backStep = backStep;
        this.active = ko.observable(false);
        this.badFile = ko.observable(false);
    }

    public saveFile(_, event): void {
        const file = event.target.files[0];
        if (file.name.match(".*.xlsx")) {
            this.file = file;
            this.badFile(false);
        } else {
            this.file = null;
            this.badFile(true);
        }
    }

    public valid(): boolean {
        return !this.badFile() && this.file !== null;
    }

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

    public getDto() {
        const fdata = new FormData();
        fdata.append(this.file.name, this.file);
        return fdata;
    }
}