// WARNING: Please don't edit this file. It was generated by C++/WinRT v2.0.190620.2

void* winrt_make_Unicord_Universal_Voice_Background_ServiceBackgroundTask()
{
    return winrt::detach_abi(winrt::make<winrt::Unicord::Universal::Voice::Background::factory_implementation::ServiceBackgroundTask>());
}
namespace winrt::Unicord::Universal::Voice::Background
{
    ServiceBackgroundTask::ServiceBackgroundTask() :
        ServiceBackgroundTask(make<Unicord::Universal::Voice::Background::implementation::ServiceBackgroundTask>())
    {
    }
}