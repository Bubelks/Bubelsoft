///<amd-module name="buildings/building/reports/reports"

import * as ko from "knockout";
import * as rest from "utils/communication/rest";
export class Reports {
    public types: ISelectValue[] = [
        {
            value: 1, displayValue: "Entire"
        },
        {
            value: 2, displayValue: "Custom"
        }];
    public type: ko.Observable<ISelectValue>;
    public showPickers: ko.Observable<boolean>;

    public estimationsReport: ko.ObservableArray<EstimationReport>;

    private sub: ko.subscription<ISelectValue>;
    private buildingId: number;

    constructor(buildingId: number) {
        this.buildingId = buildingId;
        const defaultType = this.types[0];
        this.type = ko.observable(defaultType);
        this.estimationsReport = ko.observableArray([]);
    }

    public refresh(): void {
        this.getReports().done(data => this.estimationsReport(data.map(d => new EstimationReport(d))));
    }

    private getReports(): JQueryPromise<EstimationReport[]> {
        if (this.type().value === 1)
            return rest.post("building", `${this.buildingId}/mainReports`, {});

        const fromString = $("#reports-date-from").val().split("/");
        const from = new Date(Date.UTC(fromString[2] as number, (fromString[0] as number) - 1, fromString[1] as number));
        const toString = $("#reports-date-to").val().split("/");
        const to = new Date(Date.UTC(toString[2] as number, (toString[0] as number) - 1, toString[1] as number));
        return rest.post("building", `${this.buildingId}/mainReports`,
            {
                from: from,
                to: to
            });
    }
}

interface IWorkDone {
    companyName: string;
    date: Date;
    quantity: number;
}

interface IEstimationReport {
    estimationId: string;
    specNumber: string;
    description: string;
    unit: string;
    quantity: number;
    unitPrice: number;
    amount: number;
    doneQuantity: number;
    work: IWorkDone[];
}

class EstimationReport {
    public estimationId: string;
    public specNumber: string;
    public description: string;
    public unit: string;
    public quantity: number;
    public unitPrice: number;
    public amount: number;
    public doneQuantity: number;
    public work: IWorkDone[];

    public isOpen: ko.Observable<boolean>;

    constructor(base: IEstimationReport) {
        this.estimationId = base.estimationId;
        this.specNumber = base.specNumber;
        this.description = base.description;
        this.unit = base.unit;
        this.quantity = base.quantity;
        this.unitPrice = base.unitPrice;
        this.amount = base.amount;
        this.doneQuantity = base.doneQuantity;
        this.work = base.work;
        this.isOpen = ko.observable(false);
    }

    public toggle(): void {
        this.isOpen(!this.isOpen());
    }

    public icon(): string {
        if (this.isOpen())
            return "fa-chevron-down";
        return "fa-chevron-right";
    }
}