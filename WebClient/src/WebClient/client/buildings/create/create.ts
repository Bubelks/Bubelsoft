///<amd-module name="buildings/create/create"/>

import { EstimateStep } from "buildings/create/steps/estimate";
import { BuildingStep } from "buildings/create/steps/building";
import { CompanyStep } from "buildings/create/steps/company";

import * as rest from "utils/communication/rest";
import * as ko from "knockout";
import * as navigator from "utils/navigator";

export class Create {
    private buildingId: number;
    public currentStep: ko.Observable<IStep>;
    public estimateStep: EstimateStep;
    public buildingStep: BuildingStep;
    public companyStep: CompanyStep;

    constructor(buildingId: number) {
        this.currentStep = ko.observable(null);
        this.buildingId = buildingId;
        this.getBuilding().done((data: IBuilding) => {
            this.buildingStep = new BuildingStep(data.name);
            this.estimateStep = new EstimateStep(this.buildingStep);
            this.companyStep = new CompanyStep(this.buildingStep, data.company);

            this.buildingStep.setNext(this.estimateStep);
            this.buildingStep.setBack(this.companyStep);

            this.currentStep(this.companyStep);
            this.currentStep().activate(true);
        });
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

    public cancel(): void {
        navigator.navigate("home");
    }

    public save(): void {
        var building = {
            name: this.buildingStep.name(),
            company: this.companyStep.getDto()
        }
        rest.put("buildings", `${this.buildingId}`, building)
            .done(() => navigator.navigate(`buildings/${this.buildingId}`));
    }

    private getBuilding(): JQueryPromise<IBuilding> {
        return rest.get("buildings", `${this.buildingId}`);
    }
    private changeStep(newStep: IStep): void {
        this.currentStep().activate(false);
        this.currentStep(newStep);
        this.currentStep().activate(true);
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

interface IBuilding {
    name: string;
    company: ICompany;
}

export interface ICompany {
    id: number;
    name: string;
    nip: string;
    phoneNumber: string;
    eMail: string;
    city: string;
    postCode: string;
    street: string;
    placeNumber: string;
}
