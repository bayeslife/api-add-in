
package nz.co.API.Technical.API1;

import java.util.HashMap;
import java.util.Map;
import javax.annotation.Generated;
import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.annotation.JsonValue;

@Generated("org.jsonschema2pojo")
public enum HTTPStatusCategory {

    _4_XX_CLIENT_ERROR("4XX_CLIENT_ERROR"),
    _5_XX_SERVER_ERROR("5XX_SERVER_ERROR");
    private final String value;
    private final static Map<String, HTTPStatusCategory> CONSTANTS = new HashMap<String, HTTPStatusCategory>();

    static {
        for (HTTPStatusCategory c: values()) {
            CONSTANTS.put(c.value, c);
        }
    }

    private HTTPStatusCategory(String value) {
        this.value = value;
    }

    @JsonValue
    @Override
    public String toString() {
        return this.value;
    }

    @JsonCreator
    public static HTTPStatusCategory fromValue(String value) {
        HTTPStatusCategory constant = CONSTANTS.get(value);
        if (constant == null) {
            throw new IllegalArgumentException(value);
        } else {
            return constant;
        }
    }

}
