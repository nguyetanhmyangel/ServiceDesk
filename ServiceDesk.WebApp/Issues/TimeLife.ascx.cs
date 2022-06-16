using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;

namespace ServiceDesk.WebApp.Issues
{
    public partial class TimeLife : UserControl
    {
        private readonly IIssuesRepository _issuesRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskExecuteRepository _taskExecuteRepository;

        public TimeLife(IIssuesRepository issuesRepository, ITaskRepository taskRepository, ITaskExecuteRepository taskExecuteRepository)
        {
            _issuesRepository = issuesRepository;
            _taskRepository = taskRepository;
            _taskExecuteRepository = taskExecuteRepository;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Claim.Session[Config.UserId] != null && Claim.Session[Config.RoleId] != null)
            {
                var languageId = Claim.Session[Config.LanguageId] != null ? Claim.Session[Config.LanguageId].ToString() : "vi-VN";
                var userId = Claim.Session[Config.UserId].ToString();
                try
                {
                    var issueId = Helper.ConvertToInt(Page.Items["IssueId"].ToString());
                    InitTimeLife(issueId, languageId, userId);
                }
                catch (Exception ex)
                {
                    //
                }

            }
            else
                Helper.PageRedirecting("~/Account/Login");
        }

        public void InitTimeLife(int issueId, string lang, string userId)
        {
            // Employee Issue
            var issueItems = _issuesRepository.FindForTimeLife(issueId, lang, userId).ToList();
            foreach (var issue in issueItems)
            {
                PlaceHolder1.Controls.Add(new LiteralControl("<li>"));
                PlaceHolder1.Controls.Add(new LiteralControl("<div class='timeline-datetime'>"));
                PlaceHolder1.Controls.Add(new LiteralControl("<span class='timeline-time'>" + issue.CreateDate + "</span>"));
                PlaceHolder1.Controls.Add(new LiteralControl("</div>"));

                PlaceHolder1.Controls.Add(new LiteralControl("<div class='timeline-badge blue'>"));
                PlaceHolder1.Controls.Add(new LiteralControl("<i class='fa fa-tag'></i>"));
                PlaceHolder1.Controls.Add(new LiteralControl("</div>"));

                PlaceHolder1.Controls.Add(new LiteralControl("<div class='timeline-panel'>"));
                PlaceHolder1.Controls.Add(new LiteralControl("<div class='timeline-header bordered-bottom bordered-blue'>"));
                PlaceHolder1.Controls.Add(new LiteralControl("<span class='timeline-title'>" + issue.Title + "</span>"));
                PlaceHolder1.Controls.Add(new LiteralControl("</div>"));
                PlaceHolder1.Controls.Add(new LiteralControl("<div class='timeline-body'>"));
                PlaceHolder1.Controls.Add(new LiteralControl("<p>Người yêu cầu: " + issue.EmployeeName + "</p>"));
                PlaceHolder1.Controls.Add(new LiteralControl("<p>Đơn vị: " + issue.DivisionName + "</p>"));
                PlaceHolder1.Controls.Add(new LiteralControl("</div>"));
                PlaceHolder1.Controls.Add(new LiteralControl("</div>"));
                PlaceHolder1.Controls.Add(new LiteralControl("</li>"));
            }

            if (issueItems.Any())
            {
                var i = 0;
                // Task 
                var taskList = _taskRepository.FindForTimLife(issueId).ToList();
                foreach (var task in taskList)
                {
                    PlaceHolder1.Controls.Add(i % 2 == 0 ? new LiteralControl("<li class='timeline-inverted'>") : new LiteralControl("<li >"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<div class='timeline-datetime'>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<span class='timeline-time'>" + task.CreateDate + "</span>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("</div>"));

                    PlaceHolder1.Controls.Add(new LiteralControl("<div class='timeline-badge sky'>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<i class='fa fa-bar-chart-o'></i>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("</div>"));

                    PlaceHolder1.Controls.Add(new LiteralControl("<div class='timeline-panel bordered-top-3 bordered-azure'>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<div class='timeline-header bordered-bottom bordered-blue'>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<span class='timeline-title'>Phòng tiếp nhận yêu cầu: " + task.DepartmentName + "</span>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("</div>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<div class='timeline-body'>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<p>Tình trạng: " + task.StatusName + "</p>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<p>Ngày bắt đầu: " + task.StartDate + " - Ngày kết thúc: " + task.EndDate + "</p>"));
                    if (task.UserHandleList != null)
                    {
                        PlaceHolder1.Controls.Add(new LiteralControl("<p>Người xử lí: " + task.UserHandleList + "</p>"));
                    }
                    PlaceHolder1.Controls.Add(new LiteralControl("</div>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("</div>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("</li>"));
                    PlaceHolder1.Controls.Add(new LiteralControl("<p>Ghi chú: " + task.Description + "</p>"));
                    i++;
                }

                // Task Execute
                if (taskList.Any())
                {
                    var taskIdList = taskList.Select(d => d.Id);
                    var taskExecuteList = _taskExecuteRepository.FindForTimLife(taskIdList).ToList();
                    foreach (var taskExecute in taskExecuteList)
                    {
                        PlaceHolder1.Controls.Add(i % 2 == 0 ? new LiteralControl("<li class='timeline-inverted'>") : new LiteralControl("<li >"));
                        PlaceHolder1.Controls.Add(new LiteralControl("<div class='timeline-datetime'>"));
                        PlaceHolder1.Controls.Add(new LiteralControl("<span class='timeline-time'>" + taskExecute.CreateDate + "</span>"));
                        PlaceHolder1.Controls.Add(new LiteralControl("</div>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<div class='timeline-badge red'>"));
                        PlaceHolder1.Controls.Add(new LiteralControl("<i class='fa fa-exclamation font-120'></i>"));
                        PlaceHolder1.Controls.Add(new LiteralControl("</div>"));

                        PlaceHolder1.Controls.Add(new LiteralControl("<div class='timeline-panel bordered-top-3 bordered-azure'>"));
                        PlaceHolder1.Controls.Add(new LiteralControl("<div class='timeline-header bordered-bottom bordered-red'>"));
                        PlaceHolder1.Controls.Add(new LiteralControl("<span class='timeline-title'>Người xử lí: " + taskExecute.FullName + "</span>"));
                        PlaceHolder1.Controls.Add(new LiteralControl("<i style='padding-left: 20px'>Điện thoại: " + taskExecute.Mobile + " - Số nội bộ: " + taskExecute.Phone + " - Email: " + taskExecute.Email + "</i>"));
                        PlaceHolder1.Controls.Add(new LiteralControl("</div>"));
                        PlaceHolder1.Controls.Add(new LiteralControl("<div class='timeline-body'>Tiến độ:"));
                        PlaceHolder1.Controls.Add(new LiteralControl("<div class='progress progress-striped'>"));
                        PlaceHolder1.Controls.Add(new LiteralControl("<div class='progress-bar progress-bar-palegreen' role='progressbar' aria-valuenow='" + taskExecute.Progress + "' aria-valuemin='0' aria-valuemax='" + taskExecute.Progress + "' style='width: " + taskExecute.Progress + "%'></div>"));
                        PlaceHolder1.Controls.Add(new LiteralControl("</div>"));
                        PlaceHolder1.Controls.Add(new LiteralControl("<p>Ghi chú: " + taskExecute.Description + "</p>"));
                        PlaceHolder1.Controls.Add(new LiteralControl("</div>"));
                        PlaceHolder1.Controls.Add(new LiteralControl("</div>"));
                        PlaceHolder1.Controls.Add(new LiteralControl("</li>"));
                        i++;
                    }
                }

                PlaceHolder1.Controls.Add(new LiteralControl("<li class='timeline-node'><a class='btn btn-info'>Kết Thúc</a></li>"));
            }
        }
    }
}