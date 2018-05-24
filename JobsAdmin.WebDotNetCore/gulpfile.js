var gulp = require('gulp');
var rename = require('gulp-rename');

gulp.task('bootstrap-css', function(){
    return gulp
        .src('node_modules/bootstrap/dist/css/bootstrap.min.css')
        .pipe(gulp.dest('wwwroot/lib/css'))
});

gulp.task('bootstrap-fonts', function(){
    return gulp
        .src('node_modules/bootstrap/dist/fonts/**')
        .pipe(gulp.dest('wwwroot/lib/fonts'))
});

gulp.task('bootstrap-js', function(){
    return gulp
        .src('node_modules/bootstrap/dist/js/bootstrap.min.js')
        .pipe(gulp.dest('wwwroot/lib/js'))
});

gulp.task('jquery', function(){
    return gulp
        .src('node_modules/jquery/dist/jquery.min.js')
        .pipe(gulp.dest('wwwroot/lib/js'))
});

gulp.task('knockout', function () {
    return gulp
        .src('node_modules/knockout/build/output/knockout-latest.js')
        .pipe(rename('knockout.min.js'))
        .pipe(gulp.dest('wwwroot/lib/js'))
});

gulp.task('signalr', function () {
    return gulp
        .src('node_modules/@aspnet/signalr/dist/browser/signalr.min.js')
        .pipe(gulp.dest('wwwroot/lib/js'))
});


gulp.task('default', [ 'bootstrap-css', 'bootstrap-fonts', 'bootstrap-js', 'jquery', 'knockout', 'signalr' ]); 