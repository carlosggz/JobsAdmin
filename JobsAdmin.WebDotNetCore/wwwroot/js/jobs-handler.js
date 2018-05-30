function jobItem(job) {

    this.id = ko.observable(job.id);
    this.name = ko.observable(job.name);
    this.progress = ko.observable(job.progress);
    this.statusId = ko.observable(job.status);
    this.statusName = ko.observable(JobsAdmin.jobStatus[job.status]);
}

function jobItem(job) {

    var self = this;

    this.getDateFormatted = function (date) {

        return (date.getMonth() + 1) + "/" +
            date.getDate() + "/" +
            date.getFullYear() + " " +
            date.getHours() + ":" +
            date.getMinutes();
    };

    this.id = ko.observable(job.id);
    this.name = ko.observable(job.name);
    this.progress = ko.observable(job.progress);
    this.statusId = ko.observable(job.status);
    this.statusName = ko.observable(JobsAdmin.jobStatus[job.status]);
    this.scheduledAt = ko.observable();
    this.scheduledAtMessage = ko.observable();

    this.scheduledAt.subscribe(function () {
        self.scheduledAtMessage(self.scheduledAt() ? self.getDateFormatted(self.scheduledAt()) : '');
    });

    this.scheduledAt(job.scheduledAt ? new Date(job.scheduledAt) : null);
}

function jobsHandler(config) {

    var self = this;
    this.jobs = ko.observableArray([]).extend({ deferred: true });
    this.logEntries = ko.observableArray([]);

    this.updateJob = function (notification) {

        var job = self
            .jobs()
            .find(function (job) { return job.id() === notification.id; });

        if (!job)
            return;

        job
            .progress(notification.progress)
            .statusId(notification.status)
            .statusName(JobsAdmin.jobStatus[notification.status])
            .scheduledAt(new Date(notification.ScheduledAt));

        self
            .logEntries
            .push({
                id: notification.id,
                type: JobsAdmin.notificationTypes[notification.notificationType],
                message: notification.message
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
