///<amd-module name="utils/router"/>

export class Router {
    private routes: Array<IRoute>;
    private readonly mode: RouterMode;
    private readonly root: string = "/";
    private listener: number;

    public constructor(options: IRouterOptions) {
        this.mode = options && options.mode && options.mode === RouterMode.History && !!(history.pushState)
            ? RouterMode.History
            : RouterMode.Hash;
        this.root = options && options.root ? `/${this.clearSlashes(options.root)}/` : "/";
        this.routes = new Array<IRoute>();
    }

    public add(route: string, handler: Function): Router {
        this.routes.push({ route: route, handler: handler });
        return this;
    }

    public remove(route: string): Router {
        this.routes.forEach((r, i) => {
            if (r.route === route) {
                this.routes.splice(i, 1);
            }
        });
        return this;
    }

    public navigate(path: string): void {
        path = path ? path : "";
        if (this.mode === RouterMode.History)
            history.pushState({path: path}, this.clearSlashes(path), this.clearSlashes(path));
        else     
            window.location.href = window.location.href.replace(/#(.*)$/, "") + "#" + path;
    }

    public check(fragment: string): void {
        if (fragment === null || typeof fragment === "undefined")
            fragment = this.getFragment();

        for (let i = 0; i < this.routes.length; i++) {
            var variableNames = [];
            const route = this.routes[i].route.replace(/([:*])(\w+)/g, (full, dots, name) => {
                variableNames.push(name);
                return "([^\/]+)";
            }) + "(?:\/|$)";
            const match = fragment.match(new RegExp(route));
            if (match) {
                match.shift();
                this.routes[i].handler.apply({}, match);
                return;
            }
        }
    }

    public getFragment(): string {
        let fragment: string;
        if (this.mode === RouterMode.History) {
            const match = window.location.href.match(/#(.*)$/);
            fragment = match ? match[1] : "";
        } else {
            const match = window.location.href.match(/#(.*)$/);
            fragment = match ? match[1].split("/")[0] : "";
        }
        return this.clearSlashes(fragment);
    }

    public start = () => {
        var current = this.root;
        const fn = () => {
            if (current !== this.getFragment()) {
                current = this.getFragment();
                this.check(current);
            }
        };
        clearInterval(this.listener);
        this.listener = setInterval(fn, 50);
    }

    private clearSlashes(path: string): string {
        return path.toString().replace(/\/$/, "").replace(/^\//, "");
    }
}

export enum RouterMode {
    Hash,
    History
}

export interface IRouterOptions {
    mode: RouterMode;
    root: string;
}

interface IRoute {
    route: string;
    handler: Function;
}