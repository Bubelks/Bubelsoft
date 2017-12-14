///<amd-module name="buildings/create/create"/>

import { EstimateStep } from "buildings/create/steps/estimate";
import { BuildingStep } from "buildings/create/steps/building";
import { CompanyStep } from "buildings/create/steps/company";

import * as ko from "knockout";

export class Create {
    public readonly currentStep: ko.Observable<IStep>;
    public readonly estimateStep: EstimateStep;
    public readonly buildingStep: BuildingStep;
    public readonly companyStep: CompanyStep;

    constructor() {
        this.buildingStep = new BuildingStep();
        this.estimateStep = new EstimateStep(this.buildingStep);
        this.companyStep = new CompanyStep(this.buildingStep);

        this.buildingStep.setNext(this.estimateStep);
        this.buildingStep.setBack(this.companyStep);

        this.currentStep = ko.observable<IStep>(this.companyStep);
        this.currentStep().activate(true);
    }

    public next(): void {
        //if (!this.currentStep().valid()) return;

        const nextStep = this.currentStep().next();
        if (nextStep !== null)
            this.changeStep(nextStep);
        else
            this.save();
    }

    public back(): void {
        const backStep = this.currentStep().back();
        if (backStep !== null)
            this.changeStep(backStep);
    }

    private changeStep(newStep: IStep): void {
        this.currentStep().activate(false);
        this.currentStep(newStep);
        this.currentStep().activate(true);
    }
    private save(): void {
        
    }

    public dispose(): void {
        
    }
}

export interface IStep {
    valid(): boolean;
    next(): IStep;
    back(): IStep;
    activate(active: boolean): void;
    header(): string;
}

enum Step {
    Company,
    Building,
    Estimate
}
