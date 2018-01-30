var gulp = require('gulp');

gulp.task('default', function() {

	return gulp.src('node_modules/@aspnet/signalr-client/dist/browser/signalr-client-*.js')
		.pipe(gulp.dest('wwwroot/lib/signalr'));

});
