function jobItem(job) {

    var self = this;

    this.getDateFormatted = function (date) {

        return (date.getMonth() + 1) + "/" +
            date.getDate() + "/" +
            date.getFullYear() + " " +
            date.getHours() + ":" +
            date.getMinutes();
    };

    this.id = ko.observable(job.Id);
    this.name = ko.observable(job.Name);
    this.progress = ko.observable(job.Progress);
    this.statusId = ko.observable(job.Status);
    this.statusName = ko.observable(JobsAdmin.jobStatus[job.Status]);
    this.scheduledAt = ko.observable();
    this.scheduledAtMessage = ko.observable();

    this.scheduledAt.subscribe(function () {
        self.scheduledAtMessage(self.scheduledAt() ? self.getDateFormatted(self.scheduledAt()) : '');
    });    

    this.scheduledAt(job.ScheduledAt ? new Date(job.ScheduledAt) : null);
}

function jobsHandler(config) {

    var self = this;
    this.jobs = ko.observableArray([]).extend({ deferred: true });
    this.logEntries = ko.observableArray([]);

    this.updateJob = function (notification) {

        var job = self
            .jobs()
            .find(function (job) { return job.id() === notification.Id; });

        if (!job)
            return;

        job
            .progress(notification.Progress)
            .statusId(notification.Status)
            .statusName(JobsAdmin.jobStatus[notification.Status])
            .scheduledAt(new Date(notification.ScheduledAt));

        self
            .logEntries
            .push({
                id: notification.Id,
                type: JobsAdmin.notificationTypes[notification.NotificationType],
                message: notification.Message
            });
    };

    this.addJob = function (job) {

        self.jobs.push(new jobItem(job));
    };

    this.removeJob = function (id) {

        var jobs = self
            .jobs()
            .filter(function (job) {
                return job.id() !== id;
            });

        self.jobs(jobs);
    };

    this.addSlowest = function () {

        $.post(config.addSlowestsUrl);
    };

    this.addNormal = function () {

        $.post(config.addNormalUrl);
    };

    this.addScheduled = function () {

        $.post(config.addScheduledUrl);
    };
}
