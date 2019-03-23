"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    uglify = require("gulp-uglify"),
    cleanCss = require("gulp-clean-css");

var paths = {
    webroot: "./wwwroot/",
    clientroot: "./client/"
};

paths.js = paths.clientroot + "**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.concatJsDest = paths.webroot + "js/scripts.min.js";

paths.css = paths.clientroot + "**/*.css";
paths.minCss = paths.webroot + "styles/**/*.min.css";
paths.concatCssDest = paths.webroot + "styles/styles.min.css";

paths.html = paths.clientroot + "**/*.html";
paths.htmlDest = paths.webroot + "views/";
paths.htmlDestFiles = paths.htmlDest + "**/*.html";

paths.componentsRegister = paths.webroot + "js/components.register.js";
paths.koComponents = paths.clientroot + "**/components.register.js";
paths.koComponentsLoader = paths.clientroot + "**/components.loaders.js";

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
        return gulp.src(paths.html, { base: paths.clientroot })
            .pipe(gulp.dest(paths.htmlDest));
    });

gulp.task("default", ["min:js", "min:css", "html"]);