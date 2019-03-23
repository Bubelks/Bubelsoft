﻿///<amd-module name="buildings/building/estimation/estimation"/>

import { IBuildingCompany } from "buildings/building/companies/companies";

import * as rest from "utils/communication/rest";
import * as ko from "knockout";

export class Estimation {
    public readonly estimations: ko.ObservableArray<IEstimation>;
    public readonly estimationDetails: ko.Observable<EstiationDetail>;
    public readonly modalOpen: ko.Observable<boolean>;
    public readonly companies: IBuildingCompany[];
    public readonly count: ko.Observable<number>;

    public maxPage: number;
    public readonly visiblePages: ko.ObservableArray<number>;
    public currentPage: ko.Observable<number>;
    public readonly loading: ko.Observable<boolean>;

    private readonly buildingId: number;

    constructor(buildingId: number, companies: IBuildingCompany[]) {
        this.modalOpen = ko.observable(false);
        this.estimations = ko.observableArray([]);
        this.visiblePages = ko.observableArray([]);
        this.currentPage = ko.observable(1);
        this.maxPage = 1;
        this.estimationDetails = ko.observable(null);
        this.count = ko.observable(0);
        this.companies = companies;
        this.buildingId = buildingId;
        this.loading = ko.observable(true);

        this.getEstimations(buildingId)
            .done((data: any) => {
                this.count(data.count);
                this.maxPage = (data.count / 30) + 1;
                for (var i = this.currentPage() - 4 < 1 ? 1 : this.currentPage() - 4; i < ((this.currentPage() + 4) > this.maxPage ? this.maxPage : (this.currentPage() + 4)); i++)
                    this.visiblePages.push(i);
                this.estimations(data.estimations);
            });
    }
    
    public openModal = (estimation: IEstimation) => {
        this.estimationDetails(new EstiationDetail(estimation, this.buildingId, this.companies, this.onSave));

        this.modalOpen(true);
        $("body").addClass("modal-open");
        const modal = $("#estimationModal");
        modal.addClass("show");
        modal.css("display", "block");
    }

    public changePage = (choosenPage: number) => 
    {
        this.currentPage(choosenPage);
        this.visiblePages([]);
        for (var i = this.currentPage() -4 < 1 ? 1 : this.currentPage() - 4; i < ((this.currentPage() + 4) > this.maxPage ? this.maxPage : (this.currentPage() + 4)); i++)
            this.visiblePages.push(i);
        this.getEstimations(this.buildingId)
            .done((data: any) =>
                this.estimations(data.estimations));
    }

    public closeModal= () => {
        this.estimationDetails(null);

        this.modalOpen(false);
        $("body").removeClass("modal-open");
        var modal = $("#estimationModal");
        modal.removeClass("show");
        modal.css("display", "none");
    }

    private onSave = (estimation: IEstimation) => {
        rest.post("building", `${this.buildingId}/estimation/${estimation.id}`, estimation);
        this.closeModal();
        this.getEstimations(this.buildingId)
            .done((data: any) =>
                this.estimations(data.estimations));
    }

    private getEstimations(buildingId: number): JQueryPromise<IEstimation[]> {
        this.loading(true);
        var skip = (this.currentPage() - 1) * 30;
        return rest.get("buildings", `${buildingId}/estimations?skip=${skip}&take=30`)
            .always(_ => {
                this.loading(false);
            });
    }

    public dispose(): void {
        
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

    private onSave: (estimation: IEstimation) => void;

    constructor(base: IEstimation, buildingId: number, avaiableCompanies: IBuildingCompany[], onSave: (estimation:IEstimation) => void) {
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
            companyId: this.selctedCompany().value
        });
    }

    private getReport(): JQueryPromise<any> {
        return rest.get("building", `${this.buildingId}/estimation/${this.id}/reports`);
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

interface IEstimation {
    id: number;
    estimationId: string;
    specNumber: string;
    description: string;
    unit: string;
    quantity: number;
    unitPrice: number;
    amount: number;
    companyId: number;
}