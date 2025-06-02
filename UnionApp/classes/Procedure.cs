using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionApp.classes
{
    public class Procedure
    {
        //procedure
        public string PRO_LOAD_TERMINAL_LIST { get; set; } = "hr_sel_terminal_list_union_v2h";
        public string PRO_UPD_STATUS_TERMINAL_LIST { get; set; } = "hr_upd_ter_status_union_v2";
        public string PRO_SEL_TERMINAL_CMD { get; set; } = "hr_sel_terminal_CMD_union_v2";
        public string PRO_SEL_TERMINAL_CMD_DOOR { get; set; } = "HR_sel_terminal_CMD_d_union_v2";
        public string PRO_UPD_TERMINAL_CMD { get; set; } = "HR_upd_terminal_CMD_union_v2";
        public string PRO_UPD_REALTIME_ACCESSLOG { get; set; } = "hr_upd_real_time_union_v2";
        public string PRO_UPD_GET_TIME_ACCESS_LOG { get; set; } = "hr_upd_get_time_union_v2";

        public string PRO_AUTO_GET_LOG_PERIOD { get; set; } = "hr_auto_get_access_log_v2";
        public string PRO_UPD_IMAGE_LOG { get; set; } = "hr_upd_imgage_log";
        public string PRO_INSERT_USER_DATA { get; set; } = "hr_pro_insert_user_data";
        public string PRO_INSERT_USER_LIST { get; set; } = "hr_pro_insert_user_list_UNION";
        public string PRO_AUTO_SEND_USER { get; set; } = "hr_auto_send_user_to_terminal";
        public string PRO_TRANSFER_DATA { get; set; } = "hr_upd_transfer_image_log";
        public string PRO_SEL_PCR_STATUS { get; set; } = "hr_sel_pcr_status_union_v2";

        //dành cho access time list
        public string PRO_LOAD_TIME_ZONE_LIST { get; set; } = "hr_sel_terminal_tz_union_v2";
        public string PRO_LOAD_ACCESS_TIME_LIST { get; set; } = "hr_sel_terminal_at_union_v2";
        public string PRO_LOAD_ACCCESS_TIME_GROUP_LIST { get; set; } = "hr_sel_terminal_at_gr_union_v2";
    }
}
