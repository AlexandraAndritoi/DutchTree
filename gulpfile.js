var gulp = require('gulp');
var uglify = require('gulp-uglify');
var concat = require('gulp-concat');

gulp.task("minify", function () {
    return gulp.src("wwwwroot/js/**/*.js")
        .pipe(uglify())
        .pipe(concat("dutchtree.min.js"))
        .pipe(gulp.dest("wwwwroot/dist"))
})

gulp.task('default', ["minify"])
