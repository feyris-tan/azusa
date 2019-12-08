namespace moe.yo3explorer.azusa.dex.IO
{
    internal enum DexcomCommands : byte
    {
        ACK = 1,
        NAK = 2,
        INVALID_COMMAND = 3,
        INVALID_PARAM = 4,
        INCOMPLETE_PACKET_RECEIVED = 5,
        RECEIVER_ERROR = 6,
        INVALID_MODE = 7,
        PING = 10,
        READ_FIRMWARE_HEADER = 11,
        READ_DATABASE_PARTITION_INFO = 15,
        READ_DATABASE_PAGE_RANGE = 16,
        READ_DATABASE_PAGES = 17,
        /*READ_DATABASE_PAGE_HEADER = 18,*/
        READ_TRANSMITTER_ID = 25,
        READ_LANGUAGE = 27,
        READ_DISPLAY_TIME_OFFSET = 29,
        READ_RTC = 31,
        READ_BATTERY_LEVEL = 33,
        READ_SYSTEM_TIME = 34,
        READ_SYSTEM_TIME_OFFSET = 35,
        READ_GLUCOSE_UNIT = 37,
        READ_BLINDED_MODE = 39,
        READ_CLOCK_MODE = 41,
        /*READ_DEVICE_MODE = 43, */    /* known but not fully understood */
        READ_BATTERY_STATE = 48,
        READ_HARDWARE_BOARD_ID = 49,
        /* READ_FIRMWARE_SETTINGS = 54, */    /* This doesn't return anything interesting */
        READ_ENABLE_SETUP_WIZARD_FLAG = 55,
        /* READ_SETUP_WIZARD_STATE = 57, */
    }
}