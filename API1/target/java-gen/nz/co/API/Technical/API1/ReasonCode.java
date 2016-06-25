
package nz.co.API.Technical.API1;

import java.util.HashMap;
import java.util.Map;
import javax.annotation.Generated;
import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.annotation.JsonValue;

@Generated("org.jsonschema2pojo")
public enum ReasonCode {

    FIELD_REQUIRED("FIELD_REQUIRED"),
    INVALID_FORMAT("INVALID_FORMAT");
    private final String value;
    private final static Map<String, ReasonCode> CONSTANTS = new HashMap<String, ReasonCode>();

    static {
        for (ReasonCode c: values()) {
            CONSTANTS.put(c.value, c);
        }
    }

    private ReasonCode(String value) {
        this.value = value;
    }

    @JsonValue
    @Override
    public String toString() {
        return this.value;
    }

    @JsonCreator
    public static ReasonCode fromValue(String value) {
        ReasonCode constant = CONSTANTS.get(value);
        if (constant == null) {
            throw new IllegalArgumentException(value);
        } else {
            return constant;
        }
    }

}
