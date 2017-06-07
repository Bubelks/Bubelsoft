///<amd-module name="home/buildings/buildings"/>

export class Buildings {
    public buildings: IBuilding[];

    constructor() {
    this.buildings = [
        { name: "Building1", my: true },
        { name: "Building2", my: false },
        { name: "Building3", my: false }
    ];}
}

interface IBuilding {
    name: string;
    my: boolean;
}