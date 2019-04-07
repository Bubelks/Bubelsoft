"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    uglify = require("gulp-uglify"),
    cleanCss = require("gulp-clean-css");

var paths = {
    webroot: "./wwwroot/",
    clientRoot: "./client/"
};

paths.js = paths.clientRoot + "**/*.js";
paths.ts = paths.clientRoot + "**/*.ts";
paths.jsMap = paths.clientRoot + "**/*.js.map";
paths.jsRegister = paths.clientRoot + "**/*.js.register";

paths.copyAllDest = paths.webroot + "js/";

paths.minJs = paths.webroot + "js/**/*.min.js";
paths.concatJsDest = paths.webroot + "js/scripts.min.js";

paths.css = paths.clientRoot + "**/*.css";
paths.minCss = paths.webroot + "styles/**/*.min.css";
paths.concatCssDest = paths.webroot + "styles/styles.min.css";

paths.html = paths.clientRoot + "**/*.html";
paths.htmlDest = paths.webroot + "views/";
paths.htmlDestFiles = paths.htmlDest + "**/*.html";

paths.componentsRegister = paths.webroot + "js/components.register.js";
paths.koComponents = paths.clientRoot + "**/components.register.js";
paths.koComponentsLoader = paths.clientRoot + "**/components.loaders.js";

//gulp.task("ko:components",
//    function() {
//        rimraf(paths.componentsRegister, () => { });
//        return gulp.src([
//                paths.koComponentsLoader,
//                paths.koComponents
//            ])
//            .pipe(concat(paths.componentsRegister))
//            .pipe(gulp.dest("."));
//    });

gulp.task("copyTs", function () {
    rimraf(paths.destinationCopyAll + "*", function () { });
    return gulp.src([paths.js, paths.jsMap, paths.jsRegister, paths.ts], { base: paths.clientRoot})
        //.pipe(concat(paths.destinationCopyAll))
        //.pipe(uglify())
        .pipe(gulp.dest(paths.copyAllDest));
});

gulp.task("min:js", function () {
    rimraf(paths.concatJsDest, function (){});
    return gulp.src([paths.js, "!" + paths.minJs, "!" + paths.koComponents, "!" + paths.koComponentsLoader], { base: "." })
        .pipe(concat(paths.concatJsDest))
        //.pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:css",
    function() {
        rimraf(paths.concatCssDest, function() {});
        return gulp.src([paths.css, "!" + paths.minCss], { base: "." })
            .pipe(concat(paths.concatCssDest))
            .pipe(cleanCss())
            .pipe(gulp.dest("."));
    });

gulp.task("html",
    function() {
        rimraf(paths.htmlDestFiles, function () { });
        return gulp.src(paths.html, { base: paths.clientRoot })
            //.pipe(concat(paths.htmlDest))
            .pipe(gulp.dest(paths.htmlDest));
    });

gulp.task("default", ["min:js", "min:css", "html"]);

gulp.task("debug", ["copyTs", "min:css", "html"]);