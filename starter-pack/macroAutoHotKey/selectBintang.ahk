#F1::
Send select * from  
return


#F2::
Send select * from dataEmployee  with(nolock) where employee_id = ''
return

#F3::
Send select * from dataEmployee  with(nolock) where employee_name like '`%`%'
return

#F4::
Send select * from [10.1.32.193].human_capital_ace.dbo.employee  with(nolock) where empid = ''
return

#F5::
Send select * from tpk_user  with(nolock) where user_id= ''
return

#F6::
Send select sk.skflag, sk.skeffective,sk.skno , sk.skdescription, * {Enter}from [10.1.32.115].peoplepro.dbo.pmempstatus st {Enter}join [10.1.32.115].peoplepro.dbo.pmskmaster sk on st.skid = sk.skid {Enter}where st.empid in( 	select empid from [10.1.32.115].peoplepro.dbo.pmEmployee  with(nolock) where empNIK = '005443' ) {Enter}order by sk.skeffective desc
return

#F7::
Send select * from [10.1.32.115].peoplepro.dbo.pmEmployee  with(nolock) where empNIK = ''
return

#F8::
Send select * from v_dataemp with(nolock) where empID = ''
return

#F9::
Send select top 100 * from emailqueue  with(nolock) order by entryid desc
return

#F10::
Send select * from sysobjects with(nolock) where name like '`%`%' and xtype = 'u' order by name
return

#F12::
Send select sk.skflag, sk.skeffective,sk.skno , sk.skdescription, * {Enter}from pmempstatus st {Enter}join pmskmaster sk on st.skid = sk.skid {Enter}where st.empid in( 	select empid from pmEmployee  with(nolock) where empNIK = '005443' ) {Enter}order by sk.skeffective desc
return
