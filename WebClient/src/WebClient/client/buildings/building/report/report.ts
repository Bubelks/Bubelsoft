///<amd-module name="buildings/building/report/report"/>

import * as ko from "knockout";
import * as rest from "utils/communication/rest";

export class Report {
    private buildingId: number;

    public reportId: number;
    public numberOfWorkers: ko.Observable<number>;
    public workDone: ko.ObservableArray<Work>;

    public addNew: ko.Observable<boolean>;
    public newWorkEstimate: ko.Observable<ISelectValue>;
    public newWorkQuantity: ko.Observable<number>;
    private estimations: ko.ObservableArray<ISelectValue>;

    public visibleEstimations: ko.ObservableArray<ISelectValue>;

    public canEdit: ko.Observable<boolean>;

    constructor(buildingId: number, reportId: number = null) {
        this.buildingId = buildingId;
        this.reportId = reportId;
        this.numberOfWorkers = ko.observable(null);
        this.workDone = ko.observableArray([]);
        this.addNew = ko.observable(false);
        this.newWorkEstimate = ko.observable(null);
        this.newWorkQuantity = ko.observable(null);
        this.estimations = ko.observableArray([]);
        this.visibleEstimations = ko.observableArray([]);
        this.canEdit = ko.observable(true);
        
        this.getEstimations().done(data => {
            this.estimations(data.map(d => ({
                value: d.id,
                displayValue: `${d.description} (${d.unit})`
            })));

            if (reportId !== null)
                this.getReport(reportId)
                    .done((data: IReport) => {
                        this.canEdit(data.canEdit);
                        this.numberOfWorkers(data.numberOfWorkers);
                        var date = new Date(data.date);
                        $("#report-date").val(`${date.getMonth() + 1}/${date.getDate()}/${date.getFullYear()}`);
                        data.work.forEach(w => {
                            var deleted = this.estimations.remove(i => i.value === w.estimationId)[0];
                            this.workDone.push(new Work(deleted.displayValue, w.estimationId, w.quantity, this.deleteWork));
                        });
                    });
        });
    }

    public addNewWork(): void {
        this.addNew(true);
        this.visibleEstimations(this.estimations());
    }

    public filterEstimations(_, event): void {
        var text = event.target.value.toLowerCase();
        this.visibleEstimations(this.estimations().filter(e => e.displayValue.toLowerCase().indexOf(text) > -1));
    }

    public chooseEstimation = (newEstimation: ISelectValue) => {
        this.newWorkEstimate(newEstimation);
        var button = $("#dropdown_estimations");
        button.text(newEstimation.displayValue);
        button.parent().children(".dropdown-menu").removeClass("show");
    }

    public saveAdding(): void {
        this.workDone.push(new Work(this.newWorkEstimate().displayValue,
            this.newWorkEstimate().value,
            this.newWorkQuantity(),
            this.deleteWork
        ));

        this.estimations.remove(i => i.value === this.newWorkEstimate().value);
        this.newWorkEstimate(null);
        this.newWorkQuantity(null);
        this.addNew(false);
    }

    public deleteWork = (estimationId: number) => {
        var deleted = this.workDone.remove(i => i.estimationId === estimationId)[0];
        this.estimations.push({
            value: deleted.estimationId,
            displayValue: deleted.estimationDescription
        });
    } 

    public cancelAdding(): void {
        this.newWorkEstimate(null);
        this.newWorkQuantity(null);
        this.addNew(false);
    }

    public getDto(): IReport {
        const dateString = $("#report-date").val().split("/");
        return {
            date: new Date( Date.UTC(dateString[2] as number, (dateString[0] as number) - 1, dateString[1] as number)),
            numberOfWorkers: this.numberOfWorkers(),
            work: this.workDone().map(w => { return w.getDto() }),
            canEdit: false
    }
    }

    private getEstimations(): JQueryPromise<any> {
        return rest.get("building", `${this.buildingId}/estimation`);
    }

    private getReport(reportId: number): JQueryPromise<any> {
        return rest.get("building", `${this.buildingId}/report/${reportId}`);
    }

    public dispose(): void {
        
    }
}

class Work {
    public estimationDescription: string;
    public estimationId: number;
    public quantity: EditableValue;

    private deleteWork: (id: number) => void;

    constructor(estimationDescription: string,
        estimationId: number,
        quantity: number,
        deleteWork: (id: number) => void)
    {
        this.estimationDescription = estimationDescription;
        this.estimationId = estimationId;
        this.quantity = new EditableValue(quantity);
        this.deleteWork = deleteWork;
    }

    public deleteItem(): void {
        this.deleteWork(this.estimationId);
    }

    public getDto(): IWork {
        return {
            estimationId: this.estimationId,
            quantity: (this.quantity.value() as number)
        };
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

interface IReport {
    date: Date;
    numberOfWorkers;
    work: IWork[];
    canEdit: boolean;
}

interface IWork {
    estimationId: number;
    quantity: number;
}