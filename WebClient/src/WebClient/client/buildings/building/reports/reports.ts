///<amd-module name="buildings/building/reports/reports"

import { IBuildingCompany } from "buildings/building/companies/companies";

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

    public readonly estimationDetails: ko.Observable<EstiationDetail>;
    public readonly modalOpen: ko.Observable<boolean>;
    public readonly companies: IBuildingCompany[];

    public readonly count: ko.Observable<number>;
    public maxPage: number;
    public readonly visiblePages: ko.ObservableArray<number>;
    public currentPage: ko.Observable<number>;
    public readonly loading: ko.Observable<boolean>;


    constructor(buildingId: number, companies: IBuildingCompany[]) {
        this.modalOpen = ko.observable(false);
        this.buildingId = buildingId;
        const defaultType = this.types[0];
        this.type = ko.observable(defaultType);
        this.estimationsReport = ko.observableArray([]);
        this.estimationDetails = ko.observable(null);
        this.companies = companies;

        this.visiblePages = ko.observableArray([]);
        this.currentPage = ko.observable(1);
        this.maxPage = 1;
        this.loading = ko.observable(true);
        this.count = ko.observable(0);

        this.refresh();
    }

    public refresh(): void {
        this.loading(true);
        this.getReports().done((data: any) => {
            this.visiblePages([]);
            this.count(data.count);
            this.maxPage = (data.count / 30) + 1;
            for (var i = this.currentPage() - 4 < 1 ? 1 : this.currentPage() - 4; i <= ((this.currentPage() + 4) > this.maxPage ? this.maxPage : (this.currentPage() + 4)); i++)
                this.visiblePages.push(i);
            this.estimationsReport(data.reports.map(d => new EstimationReport(d)));
            this.loading(false);
        });
    }

    private getReports(): JQueryPromise<EstimationReport[]> {
        var skip = (this.currentPage() - 1) * 25;
        if (this.type().value === 1)
            return rest.post("buildings", `${this.buildingId}/estimations/mainReports?skip=${skip}&take=25`, {});

        const fromString = $("#reports-date-from").val().split("/");
        const from = new Date(Date.UTC(fromString[2] as number, (fromString[0] as number) - 1, fromString[1] as number));
        const toString = $("#reports-date-to").val().split("/");
        const to = new Date(Date.UTC(toString[2] as number, (toString[0] as number) - 1, toString[1] as number));
        return rest.post("buildings", `${this.buildingId}/estimations/mainReports?skip=${skip}&take=25`,
            {
                from: from,
                to: to
            });
    }

    public openModal = (estimation: EstimationReport) => {
        this.estimationDetails(new EstiationDetail(estimation, this.buildingId, this.companies, this.onSave));

        this.modalOpen(true);
        $("body").addClass("modal-open");
        const modal = $("#estimationModal");
        modal.addClass("show");
        modal.css("display", "block");
    }

    public closeModal = () => {
        this.estimationDetails(null);

        this.modalOpen(false);
        $("body").removeClass("modal-open");
        var modal = $("#estimationModal");
        modal.removeClass("show");
        modal.css("display", "none");
    }

    public changePage = (choosenPage: number) => {
        this.currentPage(choosenPage);
        this.refresh();
    }

    private onSave = (estimation: IEstimationReport) => {
        rest.post("buildings", `${this.buildingId}/estimations/${estimation.estimationId}`, estimation);
        this.closeModal();
        this.refresh();
    }
}
class EstiationDetail {
    public id: number;
    public buildingId: number;
    public specNumber: string;
    public estimationId: string;
    public description: EditableValue;
    public unit: EditableValue;
    public quantity: EditableValue;
    public unitPrice: EditableValue;
    public amount: EditableValue;
    public avaiableCompany: ko.ObservableArray<ISelectValue>;
    public companyEditing: ko.Observable<boolean>;
    public selctedCompany: ko.Observable<ISelectValue>;

    public reports: ko.ObservableArray<any>;

    private onSave: (estimation: IEstimationReport) => void;

    constructor(base: EstimationReport, buildingId: number, avaiableCompanies: IBuildingCompany[], onSave: (estimation: IEstimationReport) => void) {
        this.companyEditing = ko.observable(false);
        this.reports = ko.observableArray([]);
        this.id = base.id;
        this.buildingId = buildingId;
        this.specNumber = base.specNumber;
        this.estimationId = base.estimationId;
        this.description = new EditableValue(base.description);
        this.unit = new EditableValue(base.unit);
        this.quantity = new EditableValue(base.quantity);
        this.unitPrice = new EditableValue(base.unitPrice);
        this.amount = new EditableValue(base.amount);
        this.avaiableCompany =
            ko.observableArray(avaiableCompanies.map(ac => ({ value: ac.id, displayValue: ac.name })));
        const company = this.avaiableCompany()
            .filter(c => c.value === base.companyId)[0];
        this.selctedCompany = ko.observable(company);
        this.onSave = onSave;

        this.getReport()
            .done(data => {
                data.forEach(d => d.date = new Date(d.date).toLocaleDateString());
                this.reports(data);
            });
    }

    public getCompanyName(): string {
        if (this.selctedCompany() === undefined || this.selctedCompany() === null)
            return "Company Not Set";

        return this.selctedCompany().displayValue;
    }

    public toggleCompany(): void {
        this.companyEditing(!this.companyEditing());
    }

    public saveEstimation(): void {
        this.onSave({
            id: this.id,
            estimationId: this.estimationId,
            specNumber: this.specNumber,
            description: (this.description.value() as string),
            unit: (this.unit.value() as string),
            quantity: (this.quantity.value() as number),
            unitPrice: (this.unitPrice.value() as number),
            amount: (this.amount.value() as number),
            companyId: this.selctedCompany().value,
            doneQuantity: 0
        });
    }

    private getReport(): JQueryPromise<any> {
        return rest.get("buildings", `${this.buildingId}/estimations/${this.id}/reports`);
    }
}

class EditableValue {
    public value: ko.Observable<any>;
    public editingMode: ko.Observable<boolean>;

    constructor(value: any) {
        this.value = ko.observable(value);
        this.editingMode = ko.observable(false);
    }

    public icon(): string {
        if (this.editingMode())
            return "fa-check";
        return "fa-pencil";
    }

    public toggleMode(): void {
        this.editingMode(!this.editingMode());
    }
}

interface IWorkDone {
    companyName: string;
    date: Date;
    quantity: number;
}

interface IEstimationReport {
    id: number;
    estimationId: string;
    specNumber: string;
    description: string;
    unit: string;
    quantity: number;
    unitPrice: number;
    amount: number;
    doneQuantity: number;
    companyId: number;
}

class EstimationReport {
    public id: number;
    public estimationId: string;
    public specNumber: string;
    public description: string;
    public unit: string;
    public quantity: number;
    public unitPrice: number;
    public amount: number;
    public doneQuantity: number;
    public companyId: number;

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
        this.companyId = base.companyId;
        this.isOpen = ko.observable(false);
        this.id = base.id;
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